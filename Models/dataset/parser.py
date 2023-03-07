import pandas as pd
import re

# Open the dataset file for reading and read the lines
with open('Dataset.txt', 'r') as f:
    lines = f.readlines()

# Initialize some variables to store the current name, bio, query, response, and emotion
current_name = ''
current_bio = ''
current_query = ''
current_response = ''
current_emotion = ''

# Initialize an empty list to store the data
data = []

# Loop through each line in the file
for line in lines:
    # Remove any leading or trailing whitespace from the line
    line = line.strip()

    # Use regular expressions to extract the name, bio, query, response, and emotion from the line if they are present
    name_match = re.match(r'^\S*\s*Name:\s*(.*)', line, re.IGNORECASE)
    if name_match:
        current_name = name_match.group(1).strip()
    bio_match = re.match(r'^Biography:\s*(.*)', line, re.IGNORECASE)
    if bio_match:
        current_bio = bio_match.group(1).strip()
    query_match = re.match(r'^Query\s*\d*:\s*(.*)', line, re.IGNORECASE)
    if query_match:
        current_query = query_match.group(1).strip()
    response_match = re.match(r'^Response\s*\d*:\s*(.*)', line, re.IGNORECASE)
    if response_match:
        current_response = response_match.group(1).strip()
    emotion_match = re.match(r'^Emotion:\s*(.*)', line, re.IGNORECASE)
    if emotion_match:
        current_emotion = emotion_match.group(1).strip()

        # Add the name, bio, query, response, and emotion to the data list as a dictionary
        data.append({
            'Name': current_name,
            'Biography': current_bio,
            'Query': current_query,
            'Response': current_response,
            'Emotion': current_emotion
        })

# Create a pandas data frame from the data list
df = pd.DataFrame(data)
df.to_json(r'dataset.json', orient='records', indent=2)
