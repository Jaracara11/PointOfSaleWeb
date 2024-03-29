#!/bin/bash -x

set -e

# Configuration
GIT_BRANCH="master"
DOCKER_IMAGE_NAME="pos.app"
CONTAINER_NAME="pos-web.app"
HOST_PORT_HTTP=8080
CONTAINER_PORT_HTTP=5000

# Deployment Steps
cd PointOfSaleWeb.App
git checkout "$GIT_BRANCH"
git config pull.rebase false
git pull
sudo docker stop "$CONTAINER_NAME" || true  # Ignore errors if container doesn't exist
sudo docker rm "$CONTAINER_NAME" || true    # Ignore errors if container doesn't exist
sudo docker build -t "$DOCKER_IMAGE_NAME" .
sudo docker run -d -p "$CONTAINER_PORT_HTTP:$HOST_PORT_HTTP" --restart unless-stopped --name "$CONTAINER_NAME" "$DOCKER_IMAGE_NAME"

# Cleanup
sudo docker system prune -af   # Remove all stopped containers, unused networks, and dangling images
sudo docker volume prune -f    # Remove all unused volumes

# Logging
echo "List of running Docker containers:"
sudo docker ps