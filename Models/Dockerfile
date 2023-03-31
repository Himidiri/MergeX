FROM pytorch/pytorch

WORKDIR /mergex-ai

COPY requirements.txt requirements.txt

RUN pip install -r requirements.txt

COPY api.py api.py

CMD ["python3", "api.py"]