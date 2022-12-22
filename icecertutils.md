pip install zeroc-icecertutils

iceca init

iceca create --ip 127.0.0.1 server

iceca export --password password --alias ca E:\0git\ssl\cacert.pem

iceca export --password password --alias client E:\0git\ssl\client.p12

iceca export --password password --alias server E:\0git\ssl\server.p12
