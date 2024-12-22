# RentalCar.Model
Microserviço para gestão de modelos dos automoveis da loja de aluguer de carros.

A comunicação com os serviços Category (IdCategory) e Manufacturer (IdManufacturer) é feito via messageria (RabbitMQ). No momento do registo os Ids (IdCategory e IdManufacturer) são escritos dentro as respectivas filas e cada serviços irá consultar e enviar a resposta, para saber se os dados são ou não válidos.

# Arquitectura do Projecto
![Diagrama](https://github.com/user-attachments/assets/3f4e648a-f0dd-4e5f-b633-d5fdbe90098a)


# Eums

### Motor
| Key      | Value | 
|----------|-------|
| HIBRID   | H     |
| DIESEL   | D     |
| ELECTRIC | E     |
| GASOLINE | G     |

### Transmission
| Key       | Value | 
|-----------|-------|
| AUTOMATIC |   H   |
| MANUAL    |   D   |

# Entities
### Model
| Type          | Variavel       | Descrition |
|---------------|----------------|------------|
| string        |   Id           |            |
| string        |   Name         |            |
| int           |   Year         |            |
| string        | IdCategory     |            |
| string        | IdManufacturer |            |
| DateTime      |   CreatedAt    |            |
| DateTime      |   UpdatedAt    |            |
| DateTime      |   DeletedAt    |            |
| bool          |   IsDeleted    |            |
| Transmission  |   Transmission |            |
| Motor         |   Motor        |            |
| string        |   Type         |            |

# Linguagens, Ferramentas e Tecnologias
<div align="left">
  <p align="left">
    <a href="https://go-skill-icons.vercel.app/">
      <img src="https://go-skill-icons.vercel.app/api/icons?i=cs,dotnet,mysql,rabbitmq,git,kubernetes,docker,sonarqube,swagger,postman,githubactions,aws" />
    </a>
  </p>
</div> <br/>

# Monitoramento
<div align="left">
  <p align="left">
    <a href="https://go-skill-icons.vercel.app/">
      <img src="https://go-skill-icons.vercel.app/api/icons?i=prometheus,grafana" />
    </a>
  </p>
</div> <br/>

# Observabilidade e Tracing
![Jaeger_OpenTelemetry](https://github.com/user-attachments/assets/bac7e17b-c42c-48a8-83ab-c0c3c1b0f3dc)

<br/>


