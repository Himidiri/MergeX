FROM alpine:latest

RUN apk add py3-pip

WORKDIR /backend

COPY . .

RUN pip install -r requirements.txt

CMD ["python3", "api.py"]