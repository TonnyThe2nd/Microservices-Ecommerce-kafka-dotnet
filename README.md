<body>

<h1>🚀 Event-Driven E-commerce Microservices</h1>

<p>
Sistema distribuído de e-commerce baseado em arquitetura de microservices,
utilizando <strong>.NET</strong>, <strong>Kafka</strong>, <strong>Entity Framework Core</strong>, <strong>JWT</strong> e <strong>Docker</strong>.
</p>

<hr>

<h2>📌 Visão Geral da Arquitetura</h2>

<p>
Este projeto simula um fluxo completo de pedidos em um e-commerce moderno,
utilizando comunicação assíncrona entre serviços com autenticação e autorização centralizadas.
</p>

<pre>
User API (Auth + JWT) → Order API → Kafka → Stock API → Kafka → Order API → Payment API
         ↓                    ↓                    ↓                    ↓
      JWT Token         Valida Token         Valida Token         Valida Token
</pre>

<hr>

<h2>🧱 Microservices</h2>

<h3>👤 User API</h3>
<ul>
    <li>Cadastro de novos usuários</li>
    <li>Login e geração de token JWT</li>
    <li>Gerenciamento de perfis e roles</li>
    <li>Persistência de usuários em banco isolado</li>
</ul>

<h3>📦 Order API</h3>
<ul>
    <li>Criação de pedidos (requer autenticação JWT)</li>
    <li>Persistência de Orders e OrderItems</li>
    <li>Consumo de eventos de estoque</li>
    <li>Controle de status do pedido</li>
    <li>Validação de token JWT em todas as operações</li>
</ul>

<h3>📦 Stock API</h3>
<ul>
    <li>Gerenciamento de estoque</li>
    <li>Reserva de produtos</li>
    <li>Validação de disponibilidade</li>
    <li>Publicação de eventos de processamento</li>
    <li>Validação de token JWT via middleware</li>
</ul>

<h3>💳 Payment API</h3>
<ul>
    <li>Processamento de pagamentos</li>
    <li>Integração com fluxo de pedido</li>
    <li>Atualização de status de pagamento</li>
    <li>Validação de token JWT para operações seguras</li>
</ul>

<hr>

<h2>⚙️ Tecnologias Utilizadas</h2>

<ul>
    <li>.NET 10 / ASP.NET Core</li>
    <li>Entity Framework Core</li>
    <li>Apache Kafka</li>
    <li>SQL Server</li>
    <li>JWT (JSON Web Tokens)</li>
    <li>BCrypt (hashing de senhas)</li>
    <li>Docker & Docker Compose</li>
</ul>

<hr>

<h2>🔁 Fluxo de Mensageria</h2>

<ol>
    <li>Usuário se cadastra na <strong>User API</strong> e faz login → recebe token JWT</li>
    <li>Usuário autenticado cria pedido na <strong>Order API</strong> (envia token no header)</li>
    <li>Order API valida token e persiste o pedido</li>
    <li>Publica evento <strong>order-created</strong> no Kafka</li>
    <li>Stock API consome evento e valida token JWT</li>
    <li>Valida e reserva estoque</li>
    <li>Publica evento <strong>stock-processed</strong></li>
    <li>Order API atualiza status do pedido</li>
    <li>Order API publica evento <strong>order-payment-initiated</strong></li>
    <li>Payment API processa pagamento e atualiza status final</li>
</ol>

<hr>

<h2>🔐 Autenticação e Autorização</h2>

<p>
O sistema utiliza <strong>JWT (JSON Web Tokens)</strong> para garantir segurança em todas as operações:
</p>

<ul>
    <li><strong>User API</strong>: Responsável por emissão e validação de tokens</li>
    <li><strong>Demais APIs</strong>: Validam o token JWT em cada requisição</li>
    <li><strong>Fluxo assíncrono (Kafka)</strong>: Tokens são propagados nos headers dos eventos</li>
    <li><strong>Senhas</strong>: Armazenadas com hash BCrypt</li>
</ul>

<pre>
// Exemplo de requisição autenticada
GET /api/orders
Authorization: Bearer {seu_token_jwt_aqui}
</pre>

<hr>

<h2>🗄️ Banco de Dados</h2>

<p>
Cada microservice possui seu próprio banco de dados isolado,
seguindo o princípio de <strong>Database per Service</strong>.
</p>

<ul>
    <li>UserDb → Users, Roles</li>
    <li>OrderDb → Orders, OrderItems</li>
    <li>StockDb → StockItems</li>
    <li>PaymentDb → Payments</li>
</ul>

<hr>

<h2>🐳 Execução com Docker</h2>

<pre>
docker-compose up -d
</pre>

<hr>

<h2>📦 Migrations</h2>

<p>
Cada API possui seu próprio conjunto de migrations gerenciado pelo Entity Framework Core.
</p>

<pre>
# User API
cd src/UserAPI
dotnet ef migrations add InitialCreate
dotnet ef database update

# Order API
cd src/OrderAPI
dotnet ef migrations add InitialCreate
dotnet ef database update

# Stock API
cd src/StockAPI
dotnet ef migrations add InitialCreate
dotnet ef database update

# Payment API
cd src/PaymentAPI
dotnet ef migrations add InitialCreate
dotnet ef database update
</pre>

<hr>

<h2>📈 Objetivo do Projeto</h2>

<p>
Demonstrar arquitetura escalável baseada em eventos, separação de responsabilidades,
consistência eventual, comunicação assíncrona entre serviços, além de 
<strong>autenticação centralizada via JWT</strong> e propagação de contexto de segurança
em fluxos síncronos e assíncronos.
</p>

<hr>

<h2>🛠️ Melhorias Recentes</h2>

<ul>
    <li>✅ Adição da <strong>User API</strong> com cadastro e login</li>
    <li>✅ Implementação de <strong>JWT</strong> para autenticação</li>
    <li>✅ Proteção de todas as APIs com validação de token</li>
    <li>✅ Propagação do token JWT via <strong>headers do Kafka</strong></li>
    <li>✅ Ajuste do fluxo completo: Order → Stock → Order → Payment</li>
    <li>✅ Separação de bancos de dados por serviço</li>
</ul>

<hr>

</body>
</html>
