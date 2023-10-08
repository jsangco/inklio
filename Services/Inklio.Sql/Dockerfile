FROM mcr.microsoft.com/mssql/server

# Switch to root user for access to apt-get install
USER root

# Create app directory
RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app

# Bundle app source
COPY . /usr/src/app

# Fix windows line endings
RUN sed -i 's/\r//g' /usr/src/app/import-data.sh

# Grant permissions for the import-data script to be executable
RUN chmod +x /usr/src/app/import-data.sh

EXPOSE 1433

# Switch back to mssql user and run the entrypoint script
USER mssql
ENTRYPOINT /bin/bash ./entrypoint.sh