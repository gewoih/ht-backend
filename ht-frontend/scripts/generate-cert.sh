#!/bin/bash

# Generate private key
openssl genrsa -out localhost-key.pem 2048

# Generate CSR
openssl req -new -key localhost-key.pem -out localhost.csr -subj "/CN=localhost"

# Generate self-signed certificate
openssl x509 -req -days 365 -in localhost.csr -signkey localhost-key.pem -out localhost.pem

# Clean up CSR
rm localhost.csr

echo "SSL certificate generated successfully!" 