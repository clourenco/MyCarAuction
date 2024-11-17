# MyCarAuction.Api
The solution tries to use vertical sliced architecture with CQRS, repository pattern and entity framework with an in-memory database.

The assumptions made were basically the following:
- One auction one vehicle;
- Auction closed vehicle sold;

# Endpoints

### POST /api/vehicles -> Adds a vehicle
