![.NET Core](https://github.com/anaselhajjaji/SystemdHealthcheck/workflows/.NET%20Core/badge.svg?branch=master) ![Deploy to docker hub](https://github.com/anaselhajjaji/systemdhealthcheck/workflows/Deploy%20to%20docker%20hub/badge.svg?branch=master) ![Deploy to Heroku](https://github.com/anaselhajjaji/systemdhealthcheck/workflows/Deploy%20to%20Heroku/badge.svg?branch=master)

# SystemdHealthcheck

use of systemd healthcheck mechanism

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
