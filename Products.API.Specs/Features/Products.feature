Feature: Products

Scenario: Health Check Endpoint
	Given I am a web client
	When accessing health endpoint healthCheck
	Then the response should be valid

Scenario: Products Endpoint	
	Given I am a web client
	When accessing the default endpoint api/Products
	Then the result should return Products EndPoint OK