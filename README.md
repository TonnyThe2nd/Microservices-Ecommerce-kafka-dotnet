
<body>

<h1>🚀 Event-Driven E-commerce Microservices</h1>

<p>
Sistema distribuído de e-commerce baseado em arquitetura de microservices,
utilizando <strong>.NET</strong>, <strong>Kafka</strong>, <strong>Entity Framework Core</strong> e <strong>Docker</strong>.
</p>

<hr>

<h2>📌 Visão Geral da Arquitetura</h2>

<p>
Este projeto simula um fluxo completo de pedidos em um e-commerce moderno,
utilizando comunicação assíncrona entre serviços.
</p>

<pre>
Order API  →  Kafka  →  Stock API  →  Kafka  →  Order API  → Payment Flow
</pre>

<hr>

<h2>🧱 Microservices</h2>

<h3>📦 Order API</h3>
<ul>
    <li>Criação de pedidos</li>
    <li>Persistência de Orders e OrderItems</li>
    <li>Consumo de eventos de estoque</li>
    <li>Controle de status do pedido</li>
</ul>

<h3>📦 Stock API</h3>
<ul>
    <li>Gerenciamento de estoque</li>
    <li>Reserva de produtos</li>
    <li>Validação de disponibilidade</li>
    <li>Publicação de eventos de processamento</li>
</ul>

<h3>💳 Payment API</h3>
<ul>
    <li>Processamento de pagamentos</li>
    <li>Integração com fluxo de pedido</li>
    <li>Atualização de status de pagamento</li>
</ul>

<hr>

<h2>⚙️ Tecnologias Utilizadas</h2>

<ul>
    <li>.NET 10 / ASP.NET Core</li>
    <li>Entity Framework Core</li>
    <li>Apache Kafka</li>
    <li>SQL Server</li>
    <li>Docker & Docker Compose</li>
</ul>

<hr>

<h2>🔁 Fluxo de Mensageria</h2>

<ol>
    <li>Order API cria pedido</li>
    <li>Publica evento <strong>order-created</strong> no Kafka</li>
    <li>Stock API consome evento</li>
    <li>Valida e reserva estoque</li>
    <li>Publica evento <strong>stock-processed</strong></li>
    <li>Order API atualiza status do pedido</li>
    <li>Inicia fluxo de pagamento</li>
</ol>

<hr>

<h2>🗄️ Banco de Dados</h2>

<p>
Cada microservice possui seu próprio banco de dados isolado,
seguindo o princípio de <strong>Database per Service</strong>.
</p>

<ul>
    <li>OrderDb → Orders, OrderItems</li>
    <li>StockDb → StockItems</li>
    <li>PaymentDb → Payments</li>
</ul>

<hr>

<h2>🐳 Execução com Docker</h2>

<pre>
docker-compose up -d
</pre>

<p>
Cada serviço é executado de forma independente com comunicação via Kafka.
</p>

<hr>

<h2>📦 Migrations</h2>

<p>
Cada API possui seu próprio conjunto de migrations gerenciado pelo Entity Framework Core.
</p>

<pre>
dotnet ef migrations add InitialCreate
dotnet ef database update
</pre>

<hr>

<h2>📈 Objetivo do Projeto</h2>

<p>
Demonstrar arquitetura escalável baseada em eventos, separação de responsabilidades,
consistência eventual e comunicação assíncrona entre serviços.
</p>

<hr>

</body>
</html>
