Feature: ProductJsonDataStore

This is a JSON backed data store using FlatJsonDataStore API 

Scenario: Creating Product
	Given there are no products in the repository
	When Creating a new Product with name 'aNewProduct' and colour 'AliceBlue'
	Then Should have '1' record in the data store
	And Should have a Product with name 'aNewProduct' and colour 'AliceBlue'
