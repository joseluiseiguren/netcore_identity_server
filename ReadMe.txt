
Ejemplo de IdentityServer para:
	- Validar Clientes (Client Credentials)
	- Entregar AccessToken a Clientes
	- Securizar WebApi
	- Validar Usuarios


IDENTITY SERVER
---------------
	
	GetConfiguration 
		GET -> http://localhost:5000/.well-known/openid-configuration
	
	GetAccessToken (client credentials)
		POST -> http://localhost:5000/connect/token
		BODY -> grant_type:client_credentials				
		HEADER -> Content-Type:application/x-www-form-urlencoded
				  authorization: Basic am9zZXBoMTpwZXBl
					(am9zZXBoMTpwZXBl: esto son las credendiales encriptadas en base64. Desde esta pagina se puede hacer la encriptacion https://www.base64encode.org/)
					(recordar que las credenciales a encriptar deben estar en este formato: usuario:password)
		RESPONSE -> Va a responder con un Access Token con los scopes que tiene habilitados. Se pueden ver en https://jwt.io/

	[RARO USO]
	GetAccessToken (resource owner password, el cliente solicita un access token a nombre de un usuario)
		POST -> http://localhost:5000/connect/token
		BODY -> grant_type:client_credentials
				username:alice -> (usuario a validar, y su password)
				password:password
		HEADER -> Content-Type:application/x-www-form-urlencoded
				  authorization: Basic am9zZXBoMTpwZXBl
					(am9zZXBoMTpwZXBl: esto son las credendiales encriptadas en base64. Desde esta pagina se puede hacer la encriptacion https://www.base64encode.org/)
					(recordar que las credenciales a encriptar deben estar en este formato: usuario:password)


WEB API
-------
	GET EMPLOYEES -> http://localhost:5001/api/employee
	GET CUSTOMERS -> http://localhost:5001/api/customer
	HEADER -> authorization: Bearer [access token obtenido en el Post al Identity Server]



TIPS
- Ejecutar la solucion (F5). Esto iniciara los 3 proyectos en forma simultanea
- El Identity Server esta configurado para realizar 2 tareas independientes
	- Proteger web site son login de usuarios
	- Proteger web api pidiendo credenciales a clientes que la quieran consumir


PRUEBAS Client Credentials
--------------------------
	Usuario Joseph1, accesos
		- http://localhost:5001/api/employee
		- http://localhost:5001/api/customer
		- authorization: Basic am9zZXBoMTpwZXBl

	Usuario Joseph2, accesos
		- http://localhost:5001/api/customer
		- authorization: Basic am9zZXBoMjpwZXBl



Resources
	http://docs.identityserver.io/en/release/intro/big_picture.html


TEORIA
	- Hay 2 tipos de Tokens
		* self-contained: 
			La web api sabe exactamente como validar el Token, y no necesita del Identoty Server. Desventaja: es mas dificil de revocar (sin que haya vencido)
		* reference: 
			El Identity Server tiene un store de Access Tokens (solo se guardan en este lugar), y le devuelve a los clientes un ID de dicho Token. Luego el cliente para saber si el Token es valido, debe consultar al IDS en cada request que recibe
	- El tipo de Token que van a usar los clientes, se configuran justamente en el Cliente (Client.AccessTokenType)

