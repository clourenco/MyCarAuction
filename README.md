# MyCarAuction.Api

The solution tries to use vertical sliced architecture with CQRS, repository pattern and entity framework with an in-memory database.

The assumptions made were basically the following:

- One auction one vehicle;
- Auction closed vehicle sold;

# Endpoints

## Vehicles

### POST /api/vehicles -> Adds a vehicle

### GET /api/vehicles -> Searches for a vehicle

### GET /api/vehicles/{id} -> Gets a vehicle by id

## Auctions

### POST /api/auctions -> Adds an auction

### POST /api/auction/bid -> Places a bid

### PUT /api/auction/{id} -> Closes an auction

### GET /api/auctions/{id} -> Gets an auction by id

## Users

### POST /api/users -> Adds a user

### GET /api/users -> Searches for a user

### GET /api/users/{id} -> Gets a user by id
