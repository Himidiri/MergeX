# Import required libraries and modules
from flask import Flask, jsonify, request, send_from_directory
from flask_pymongo import PyMongo
from werkzeug.security import generate_password_hash, check_password_hash
import os
from flask_cors import CORS

# Create a Flask app and set MongoDB URI in configuration
app = Flask(__name__, static_folder='build')
app.config["MONGO_URI"] = os.getenv("MONGO_URI")
mongo = PyMongo(app)

# Set up CORS to allow only requests from a specific domain
cors = CORS(app, resources={r"/*": {"origins": os.getenv("ALLOWED_ORIGIN")}}, methods=["POST"])

# Define a sign-up route that creates a new user in the database
@app.route("/signup", methods=["POST"])
def signup():
    # Extract user data from request body
    name = request.json.get("name")
    email = request.json.get("email")
    password = request.json.get("password")

    # Hash the password for security and set user status to approved
    hashed_password = generate_password_hash(password)
    approved = True

    # Create a dictionary object for the new user
    user = {"name": name, "email": email, "password": hashed_password, "approved": approved}

    # Insert the new user into the database
    mongo.db.users.insert_one(user)

    # Return a success message to the client
    return jsonify({"message": "User created successfully."})

# Define a log-in route that authenticates the user and returns a token if successful
@app.route("/login", methods=["POST"])
def login():
    # Extract user credentials from request body
    email = request.json.get("email")
    password = request.json.get("password")

    # Search for the user in the database by email
    user = mongo.db.users.find_one({"email": email})

    # Verify the user's password and status
    if user and check_password_hash(user["password"], password):
        if user["approved"]:
            # Return a success message and token to the client
            return jsonify({"message": "Logged in successfully."})
        else:
            # Return an error message if the user is not approved
            return jsonify({"message": "User is not approved."}), 401
    else:
        # Return an error message if the email or password is invalid
        return jsonify({"message": "Invalid email or password."}), 401

# Health Check
@app.route("/healthz", methods=["GET"])
def healthz():
    return jsonify({"success": True}), 200

# Serve React App
@app.route('/', defaults={'path': ''})
@app.route('/<path:path>')
def serve(path):
    if path != "" and os.path.exists(app.static_folder + '/' + path):
        return send_from_directory(app.static_folder, path)
    else:
        return send_from_directory(app.static_folder, 'index.html')

# Run the app in debug mode if it's the main module
if __name__ == '__main__':
    app.run(use_reloader=True, port=5000, threaded=True, host="0.0.0.0")
