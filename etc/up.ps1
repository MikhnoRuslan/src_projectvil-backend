podman network create projectiv-network
podman-compose -f docker-compose.infrastructure.yml up -d

Write-Host "Press Enter to close..."
Read-Host