FROM alpine:latest

RUN apk add py3-pip

WORKDIR /backend

COPY requirements.txt requirements.txt

RUN pip install -r requirements.txt

COPY api.py api.py

CMD ["python3", "api.py"]