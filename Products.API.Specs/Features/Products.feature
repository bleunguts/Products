Feature: Products

Scenario: Products Index Endpoint		
	When accessing the endpoint 'api/Products/ping'
	Then the response should have a valid http status code
	And the result should return 'ProductsController OK'

Scenario: Create a Product
	Given there are no products in the repository
	When Creating a new Product with name 'aProductName' with colour 'AliceBlue'
	Then the response should have a valid http status code

Scenario: Create a Product without specifying required fields
	Given there are no products in the repository
	When Creating a new Product without required arguments
	Then the response should have a 'BadRequest' http status code
	And the response should contain error message detailing problem 'The Name field is required.'

Scenario: Create a Product without specifying a colour
	Given there are no products in the repository
	When Creating a new Product with name 'aProductName'
	Then the response should have a valid http status code

Scenario: Get Product List 
	Given there are some products in the repository
	When accessing the endpoint 'api/Products'
	Then the response should have a valid http status code
	And the result should return some products

Scenario: Get Product List Filtered By Colour
	Given there is a 'Blue' product in the repository
	When accessing the endpoint 'api/Products' with query 'FilterColourBy' set to 'Blue'
	Then the response should have a valid http status code
	And the result should return a product which has colour 'Blue'

Scenario: Get Product List Filtered By Colour That Does Not Exist
	Given there are some products in the repository
	When accessing the endpoint 'api/Products' with query 'FilterColourBy' set to 'Blue'
	Then the response should return a 'NotFound' http status code and an error message that contains 'FilterColourBy: Blue'

Scenario: Get Product List Filtered By Colour Case insensitive
	Given there is a 'blue' product in the repository
	When accessing the endpoint 'api/Products' with query 'FilterColourBy' set to 'Blue'
	Then the response should have a valid http status code
	And the result should return a product which has colour 'Blue'

Scenario: Get Product List when there are no products
	Given there are no products in the repository
	When accessing the endpoint 'api/Products'
	Then the response should have a valid http status code
	And the result should return no products