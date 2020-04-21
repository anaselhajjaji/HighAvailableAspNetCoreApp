![CI CD](https://github.com/anaselhajjaji/systemdhealthcheck/workflows/CI%20CD/badge.svg?branch=master) [![Build Status](https://dev.azure.com/elhajjajianas/HighAvailableAspNetCoreApp/_apis/build/status/anaselhajjaji.high-available-aspnetcore?branchName=master)](https://dev.azure.com/elhajjajianas/HighAvailableAspNetCoreApp/_build/latest?definitionId=8&branchName=master)

# SystemdHealthcheck

Use of systemd healthcheck mechanism, and mediator design pattern for communication between the API and the components as well as between HosterServices.

Health Check available at: /health

Health Check UI available at: /health-ui

Swagger available at : /swagger

Serilog is used for logging in console as well as publishing logs to a server.

CI/CD example using Gihub Actions, Azure Pipelines and Gitlab CI:
- Unit testing
- Publish to Heroku
- API testing using newman
- Publish to Docker HUB

## Project initialization

Initial project created with command: `dotnet new webapi --no-https`

## Docker support

Done in Visual Studio Code using :

Command Palette `(Ctrl+Shift+P)` and enter `Docker: Add Docker Files to Workspace....` 

For debug: you can instead do `Docker: Initialize for Docker debugging`

### Docker health check

Added to Dockerfile: `HEALTHCHECK CMD curl --fail http://localhost:80/health || exit 1`

To test: `docker inspect --format='{{json .State.Health}}' CONTAINER_ID`

## Build for linux

To build for Raspberry Pie 3: `dotnet publish -c Release -r linux-arm64 --self-contained true`

## systemd support

Move the file SystemdHealthcheck.service to: `/etc/systemd/system/SystemdHealthcheck.service`

Then: `sudo systemctl start SystemdHealthcheck`

To see the status: `sudo systemctl status SystemdHealthcheck`

## Give gitlab-runner sudo permission

`sudo usermod -a -G sudo gitlab-runner`

then,
`sudo visudo`

and add to the bottom of the file:
`gitlab-runner ALL=(ALL) NOPASSWD: ALL`

