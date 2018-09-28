
Ejemplo de IdentityServer para:
	- Validar Clientes (Client Credentials) -> aplicaciones que quieren consumir la webapi
	- Validar Usuarios -> usuarios que quieren hacer login en cualquiera de los 2 websites


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


PRUEBAS Usuarios Login
----------------------
	Usuario alice
		- Tiene rol admin1 y puede ingresar a los 2 websites
		- http://localhost:5002
		- http://localhost:5003		

	Usuario bob
		- Tiene rol user1 y solo puede ingresar al website:
		- http://localhost:5002		

		
LOGGING
-------
	- Se utiliza NLog


Enviroment Variables
--------------------
	Desde PowerShell:
		$Env:ASPNETCORE_ENVIRONMENT = "Development" ---> dura solo por la session del power shell
		[Environment]::SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development", "User") ---> para que se impacten los cambios hay que cerrar el power shell y volver a abrirlo
	Tips
		- Para utilizar (por ejemplo) el connection string de la variable de entorno, y no desde el archivo "appsettings.json", solo basta con crear una variable de entorno con el mismo nombre
			[Environment]::SetEnvironmentVariable("ConnectionStrings:users_and_clients_database", "string de conexion de produccion", "User")
			[Environment]::SetEnvironmentVariable("ConnectionStrings:users_and_clients_database", $null, "User") --> elimina la variable de entorno y la app vuelve a usar el connection srting del archivo "appsettings.json"


Publish
-------
	Desde una consola:
	Parametro 1: -c -> configuration
	Parametro 2: -o -> output direcotry (si no exsiste lo crea)
	Parametro 3: proyecto que queremos publicar
	Ejemplo:
		dotnet publish -c Release -o Publish IdentServer/IdentServer.csproj



Ejecutar Aplicación
-------------------
	Desde una consola
	Pararse en la carpeta de publicacion
		dotnet .\IdentServer.dll


Resources
	http://docs.identityserver.io/en/release/intro/big_picture.html


TEORIA
	- Hay 2 tipos de Tokens
		* self-contained: 
			La web api sabe exactamente como validar el Token, y no necesita del Identoty Server. Desventaja: es mas dificil de revocar (sin que haya vencido)
		* reference: 
			El Identity Server tiene un store de Access Tokens (solo se guardan en este lugar), y le devuelve a los clientes un ID de dicho Token. Luego el cliente para saber si el Token es valido, debe consultar al IDS en cada request que recibe
	- El tipo de Token que van a usar los clientes, se configuran justamente en el Cliente (Client.AccessTokenType)

