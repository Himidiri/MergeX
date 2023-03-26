from flask import Flask, jsonify, request
from transformers import AutoTokenizer, AutoModelForSeq2SeqLM
import re

max_source_length = 256
max_target_length = 128

model = AutoModelForSeq2SeqLM.from_pretrained("amaydle/mergex")
tokenizer = AutoTokenizer.from_pretrained("google/flan-t5-base")


app = Flask(__name__)

@app.route('/', methods=['POST'])
def predict():
    content = request.get_json(silent=True)

    input_text = f"Biography: {content['bio']}\n\nQuestion: {content['question']}"

    inputs = tokenizer(input_text, max_length=max_source_length, truncation=True, return_tensors="pt")
    output = model.generate(**inputs, num_beams=8, do_sample=True, min_length=10, max_length=max_target_length)
    decoded_output = tokenizer.batch_decode(output, skip_special_tokens=True)[0]
    regex_pattern = r"Emotion:\s*(\w+)\s+Answer:\s*(.+)"
    matches = re.search(regex_pattern, decoded_output)

    if matches:
        emotion = matches.group(1)
        answer = matches.group(2)
        return jsonify({"emotion": emotion.lower(), "answer": answer})
    else:
        return jsonify({"emotion": "sad", "answer": "Sorry, I didn't understand"})

app.run(host="0.0.0.0")