Feature: HealthCheck

Scenario: Anonymous endpoint: Health check/OK endpoint
	When accessing the endpoint 'healthCheck'
	Then the response should have a valid http status code
	And the result should return 'Healthy'	
