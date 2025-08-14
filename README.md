Docker ile Hızlı Kurulum
En kolay ve hızlı yöntem Docker kullanmaktır. Aşağıdaki komut ile RabbitMQ Docker imajını indirip çalıştırabilirsiniz:

docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 -v rabbitmq_data:/var/lib/rabbitmq -e RABBITMQ_DEFAULT_USER=admin -e RABBITMQ_DEFAULT_PASS=admin rabbitmq:3-management
