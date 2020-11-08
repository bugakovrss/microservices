## microservices

This solution illustrate interconnection between several API services.

This project uses the following patterns and technologies:
- Service discovery (consul)
- IdentityServer 4 / SSO
- Retry and Circuit breaker (Polly)
 
 ## Quick start
 - Download this solution from git
 - Run this solution in docker using command 
    ```
    docker-compose up
    ```
 - Open link "http://localhost:8500" in browser and make sure that all services is available
 - Open link "http://localhost:8088/swagger/index.html" and autenticate using credential:
 
		     ```
		     {
				"login": "admin",
				"password": "admin",
				"scopes": [
				  "eventlogapi",
				  "controlapi"
				]
		      }
			```
 - Go to swagger smarthome-controlapi-app "http://localhost:8080/swagger/index.html" and log in using the previously received bearer token	
 
 ## Simulate network corruptions
 - Execute in console command:
```
  	docker exec -it smarthome-eventlogapi /bin/sh
```
 - Run in openned terminal command
    ``` 
    tc qdisc add dev eth0 root netem loss 50% 50% 
    ```
    
    to change the level loss

    ```
    tc qdisc change dev eth0 root netem loss 30% 30% 
    ```
 - Go to link "http://localhost:8080/swagger/index.html" and try execute API methods
    + /api/v1/Devices/{deviceId}/Events
    + /api/v1/Devices/TurnOn/{deviceId}
    + /api/v1/Devices/TurnOff/{deviceId}
