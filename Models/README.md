### Usage

#### Install dependencies

```sh
pip3 install -r requirements.txt
```

#### Start server

```sh
python3 api.py
```

#### Query Model

```sh
curl -X POST \
  http://localhost:5000/ \
  -H 'Content-Type: application/json' \
  -d '{
    "bio": "Hinata was soft-spoken and polite, always addressing people with proper honourifics.",
    "question": "What do you like to do for fun?"
}'
```