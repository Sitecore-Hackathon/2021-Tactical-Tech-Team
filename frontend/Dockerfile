# escape=`
# todo: improve this Docker file

FROM mcr.microsoft.com/windows/servercore:1909

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# download remote assets
RUN New-Item -Path 'C:\\downloads' -ItemType 'Directory' -Force | Out-Null; `
    & curl.exe -sS -L -o c:\\downloads\\node.msi https://nodejs.org/dist/v14.14.0/node-v14.14.0-x64.msi;

RUN Start-Process msiexec.exe -ArgumentList '/i', 'C:\\downloads\\node.msi', '/quiet', '/norestart' -NoNewWindow -Wait;`
    Remove-Item -Path 'C:\\downloads' -Force -Recurse;

# use app folder 
RUN mkdir -p C:\\app
WORKDIR C:\\app

# Use docker caching layer to speed package install
COPY package.json ./
RUN npm i

COPY . ./

RUN npm run build

ENV HOST 0.0.0.0
ENV PORT 3000

CMD ["npm", "start"]