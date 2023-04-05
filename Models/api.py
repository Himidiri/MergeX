from flask import Flask, request
from transformers import pipeline, AutoModelForSeq2SeqLM, AutoTokenizer
import re

# Load the T5 model and tokenizer
model = AutoModelForSeq2SeqLM.from_pretrained("amaydle/mergex-v2")
tokenizer = AutoTokenizer.from_pretrained("google/flan-t5-base")

# Define the pipeline with the T5 model and tokenizer
t5_pipeline = pipeline(
    "text2text-generation",
    model=model,
    tokenizer=tokenizer
)


app = Flask(__name__)

@app.route('/', methods=['POST'])
def predict():
    try:
        content = request.get_json(silent=True)

        input_text = f"Biography: {content['bio']}\n\nQuestion: {content['question']}"

        output_text = t5_pipeline(input_text, max_length=128)[0]["generated_text"]

        regex_pattern = r"Emotion:\s*(\w+)\s+Answer:\s*(.+)"
        matches = re.search(regex_pattern, output_text)

        if matches:
            emotion = matches.group(1)
            answer = matches.group(2)
            return {"emotion": emotion.lower(), "answer": answer}
        else:
            return {"emotion": "sad", "answer": "Sorry, I didn't understand"}
    except Exception:
        return {"emotion": "sad", "answer": "Sorry, Something is wrong with my brain"}, 500


app.run(host="0.0.0.0", port=5000)