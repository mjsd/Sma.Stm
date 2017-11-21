FROM nginx

COPY ca.pem /etc/nginx/ca.pem
COPY cert.pem /etc/nginx/cert.pem
COPY key.pem /etc/nginx/key.pem
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 443 444
CMD ["nginx", "-g", "daemon off;"]
