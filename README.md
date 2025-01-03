# Intaker.TaskManagementSystem

To run the application:
   
1. Run rabbitmq server

  ```
  docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4.0-management
  ```

  Documentation: https://www.rabbitmq.com/docs/download
   
2. Run Intaker.TaskManagementSystem.API project.
