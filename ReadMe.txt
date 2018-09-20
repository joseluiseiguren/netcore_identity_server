
Ejemplo de IdentityServer para:
	- Validar Clientes
	- Entregar AccessToken a Clientes
	- Securizar WebApi
	- NO trabaja con usuarios


IDENTITY SERVER
---------------
	
	GetConfiguration 
		GET -> http://localhost:54261/.well-known/openid-configuration
	
	GetAccessToken
		POST -> http://localhost:54261/connect/token
		BODY -> grant_type:client_credentials
				scope:APIEmployee 
					(APIEmployee: es el recurso a consumir)

		HEADER -> Content-Type:application/x-www-form-urlencoded
				  authorization: Basic am9zZXBoMTpwZXBl
					(am9zZXBoMTpwZXBl: esto son las credendiales encriptadas en base64. Desde esta pagina se puede hacer la encriptacion https://www.base64encode.org/)
					(recordar que las credenciales a encriptar deben estar en este formato: usuario:password)


WEB API
-------
	GET -> http://localhost:56793/api/employee
	HEADER -> authorization: Bearer [access token obtenido en el Post al Identity Server]



PASOS
0 - Ejecutar los 2 Proyectos (no importa el orden)
1 - Verificar que el Identiry Server este arriba (GetConfiguration)
2 - Pedirle al Identity Server un Access Token para poder consumir la WebApi (GetAccessToken)
3 - Consumir la WebApi

