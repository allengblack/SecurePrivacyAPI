# SecurePrivacyAPI

This repo contains three projects. 
#### The API project contains an ASP NET 5.0 Web REST API that:
- Has basic CRUD functionality for a Person model
- Uses MongoDB using C# driver
- Demonstrates MongoDB's Aggregation on the `api/people/stats` endpoint.

#### The Test Project contains tests for:
- The Controller and its functions
- The `PersonService` used in the Controller

#### The BinaryString Project contains an implementation of a function that solves the following problem: 
```
We define the following: 
- A binary string is a string consisting only of 0's and/or 1's. For example, 01011, 1111, and 00 are all binary strings.
- Theprefix of a string is any substring of the string that includes the beginning of the string. For example, the prefixes of 11010 are 1, 11, 110, 1101, and 11010.

We consider a non-empty binary string to be good if the following two conditions are true:
1. The number of 0's is equal to the number of 1's.
2. For every prefix of the binary string, the number of 1's should not be less than the number of 0's.

For example, 11010 is not good because it doesn't have an equal number of 0's and 1's, but 110010 is good because it satisfies both of the above conditions.
Create a function that takes as input a binary string and outputs whether it’s a a good binary string.
```

Thanks for checking out my code.