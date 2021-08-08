# BTCPay Server - Stream payment gateway
Payment gateway integrated with BTCPay server and streaming platform Streamlabs.

# Features
## Donate form modding
Custom styles can be applied to the donate form by uploading a custom stylesheet. You can upload a custom logo, set the page title, define default language and default currency as well.

## Google Analytics support
Set your Google Analytics identifier to track your donors.

# Installation
The easiest and the recommended way to run *btcpayserver-stream* payment gateway is via Docker.

1. Register new application on streamlabs.com/dashboard (see https://dev.streamlabs.com/docs/register-your-application for further informations) \
As redirect URL use https://{yoursite}/oauth/streamlabs/callback.

2. Pull the image
```
docker pull honzanoll/btcpayserver-stream:1.0.2
```
3. Start single node with Docker
```
docker run -p 80:5000 -e "DatabaseProvider={UsedDatabaseProvider}" -e "ConnectionStrings:NpgsqlConnection={YourConnectionString}" -e "GlobalSettings:Infrastructure:FEUrl={YourHost}" -e "StreamlabsSettings:ClientId={YourStreamlabsAppClientId}" -e "StreamlabsSettings:ClientSecret={YourStreamlabsAppSecret}" -v {PathToStorage}:/Storage -v {PathToDatabase}:/Database honzanoll/btcpayserver-stream:1.0.2
```

Alternatively, you can use docker compose
```
version: '3.7'
 
services:
  portal:
    image: "honzanoll/btcpayserver-stream:1.0.2"
  environment:
      - DatabaseProvider={UsedDatabaseProvider}
      - ConnectionStrings:NpgsqlConnection={YourConnectionString}
      - GlobalSettings:Infrastructure:FEUrl={YourHost}
      - StreamlabsSettings:ClientId={YourStreamlabsAppClientId}
      - StreamlabsSettings:ClientSecret={YourStreamlabsAppSecret}
  volumes:
      - {PathToStorage}:/Storage
      - {PathToDatabase}:/Database
  restart: always
```
Then start the composition
```
docker-compose up
```

## Application configuration
The application requires some necessary configuration attributes, which are listed below.

### **Database provider**
The application uses SQLite database by default. In this case, you don't have to specify *DatabaseProvider* and *ConnectionString*. If you want to use PostgreSQL database you have to set *DatabaseProvider* to **Npgsql** and *ConnectionString* to your database.

### **Host URL**
The hostname of your website. Must be same as redirect URL set in Streamlabs application during registration.

### **Streamlabs auth**
Your Streamlabs application client identifiers obtained in step one.

### **Persisted storage**
Some data (uploaded files) must be persisted out of the container. In case you use SQLite you have to persist the database files as well.