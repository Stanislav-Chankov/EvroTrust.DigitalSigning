1. API Gateway
2. Circuit Breaker
3. Database per Service
4. CQRS (Command Query Responsibility Segregation)

🛠️ Real-World Use Case
Imagine you're building an online food delivery platform:
- API Gateway - routes requests to services like Order, Menu, Payment, and Delivery.
- Circuit Breaker - protects the system if the Payment service goes down.
- Saga - coordinates order placement across Inventory, Payment, and Delivery.
- Database per Service - Each service has its own database to avoid tight coupling. 