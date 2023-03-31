# Import required libraries and modules
from flask import Flask, jsonify, request
from flask_pymongo import PyMongo
from werkzeug.security import generate_password_hash, check_password_hash
import os

# Create a Flask app and set MongoDB URI in configuration
app = Flask(__name__)
app.config["MONGO_URI"] = os.getenv("MONGO_URI")
mongo = PyMongo(app)

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

# Run the app in debug mode if it's the main module
if __name__ == "__main__":
    app.run(debug=True)
