#!/bin/bash -x

cd PointOfSaleWeb.App
git checkout master
git pull
docker stop pos-web.app
docker rm pos-web.app
docker build -t pos.app .
docker run -d -p 5000:5000 --restart unless-stopped --name pos-web.app pos.app
docker image prune -f

echo "List of running Docker containers:"
docker ps