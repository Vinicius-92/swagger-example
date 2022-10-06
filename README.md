# Como versionar e documentar uma API com Swagger

## Docmentar é preciso

É sempre muito bom quando vamos consumir alguma tecnologia e temos uma documentação vasta, bem escrita e com exemplos, nesse artigo vou descrever como combinar a implementação de documentações com Swagger em APIs que aplicam o versionamento dos seus recursos, dessa forma mantemos nossa fonte de código e documentações sempre juntas, previnindo a necessidade de alterar em dois pontos quando houver a necessidade de evoluir algum recurso. Isso é bom para quem irá consumir nossas APIs e também para nossos colegas de trabalho que poderão com mais facilidade trabalhar e evoluir o que estamos contruindo hoje.
Existem diversas formas e tutoriais de como realizar essa documentação, uns mais e outros menos completos, sobre o que pode ser feito e aqui irei trazer a forma que eu acho interessante utilizar com exemplos de código e explicações de uma maneira mais direta de cada trecho para ficar fácil seguir junto. O código fonte vai estar disponível em um repositório no final do artigo.

### Receita do bolo

Nesse exemplo vou utilizar uma API criada com .NET 6 que já vem com o pacote básico do Swagger, vamos adicionar mais alguns pacotes relacionados ao Swagger e também ao versionamento. Vou deixar abaixo os pacotes utilizados e também a estrutura básica do projeto, que será uma API de cadastros de alunos.

{Print da tela do csproj + scaffolding}

Podemos ver que é uma API de exemplo bastante simples com apenas uma entidade e um controller que é o suficiente para detalharmos o que é possível gerar de documentação.

#### Pacotes:

Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
Microsoft.AspNetCore.OData.Versioning.ApiExplorer
Swashbuckle.AspNetCore.Annotations

<url csproj>

## Documentando entidades

O primeiro passo que teremos para documentar nossas APIs é documentar nossas entidades, nesse exemplo estamos tratando direto da entidade de negócio, porém poderia ser um DTO ou qualquer outra classe que utilizamos para entrada ou saída de dados. Ao realizar a documentação com exemplos dessa classe após configurar nosso Swagger isso será exibido na UI e também usada para gerar os exemplos de entrada e retorno.

{Colagem model}

Escolher como nomear qualquer coisa durante o desenvolvimento é sempre um desafio, ser conciso para definir uma parte de nossa aplicação que poderá ser lido diversas pessoas que podem não ter o mesmo contexto que nós sobre o tema, pode causar problemas conforme os projetos vão ficando antigos, maiores e os times vão se renovando.
Podemos ver no recorte a forma que é possível uma classe ser documentada e seu resultado no swagger gerado, podemos melhorar o entendimento e facilitar o consumo dessa API através das descrições e exemplos, as vezes até encurtando a distância entre idiomas para que conteúdos não sejam perdidos ou mal interpretados durante o processo de tradução e entendimento dos termos.
Para gerar as linhas acima dos atributos podemos apenas digital três barras e a IDE já disponibiliza a tag XML de summary, podemos adicionar a parte de exemplo para ter mais completa essa definição e temos diversas outras tags possíveis, porém somente com essas duas já temos muita riqueza na documentação.
Para que isso seja reconhecido pelo Swagger precisaremos fazer umas configurações adicionais que serão tratadas mais abaixo.

<url classe aluno>

### Configurando o versionamento da API

Adicionar o versionamento em nossa API é bastante simples, poderíamos fazer diretamente na nossa Program.cs, porém por questão de gosto e organização, vamos usar conforme a nossa imagem inicial em uma classe separada dentro de um diretório ApiVersioning que por sua vez está no diretório Config. 

{doc02 - imagem classe ApiVersioningExtensions}

Nessa classe iremos extender a IServiceCollection para adicionar as configurações aos serviços gerenciados pela nossa classe Program.cs, essas configurações estão definindo qual é a versão padrão da nossa api, em seguida dizendo para assumir a mesma quando não for especificada pelo consumidor e avisar nas chamadas quais as versões disponíveis para quem esteja recebendo esse retorno.
Logo abaixo estamos configurando o formato de nosso versionamento para ser a letra 'v' e mais dígitos que possam estar a frente para definir o número.

Fazendo isso, temos que adicionar essa configuração na nossa classe Program através da linha de código abaixo:

{doc03 - imagem program.cs adicionando versionamento}
<url program.cs>

Com isso já poderemos avançar para a configuração do Swagger em nossa aplicação.

### Configurando Swagger

Agora chegamos a parte que será a mais extensa de configurações que é a do Swagger para trabalhar junto com o nosso versionamento de forma automática, para que assim sempre que formos adicionando novas versões de nossos controllers não termos trabalhos manuais adicionais que adicionam um ponto de falha em casa de esquecimento ou falta de procedimento padrão.
Antes de entrarmos diretamente nas classes de configuração temos que adicionar algumas linhas em nossa classe .csproj, que servirão para gerar um arquivo xml das descrição de nossas entidades geradas na primeira parte desse artigo, são elas as em destaque amarelo.

{doc04 - csproj}
<url csproj>

Após vamos criar as classes específicas para organizar melhor nosso código, essas classes serão a ConfigureSwaggerOptions.cs e SwaggerExtensions.cs, ambas dentro do diretório Swagger dentro também do Config citado anteriormente.
Vamos começar com a ConfigureSwaggerOptions pois precisaremos dessa na seguinte, segue abaixo as configurações e falarmos brevemente ponto a ponto em seguida:

{doc05 - imagem classe ConfigureSwaggerOptions}
<url classe acima>

Essa classe é bastante longa vamos, como fazia Jack por partes, se compararmos com as que geramos anteriores, porém a maior parte dela é código boilerplate que iremos ver e rever nas aplicações sempre na mesma estrutura. Vamos agora ponto a ponto seguindo a ordem da classe para entender melhor sua construção.
O primeiro ponto que vemos é que a classe implementa a interface IConfigureNamedOptions do tipo SwaggerGenOptions, através dessa interface ela nos obriga a implementar dois métodos de configuração e através deles construímos a nossa classe.
{doc06}
Após isso podemos ver que vamos realizar injetar uma IApiVersionDescriptionProvider através do nosso construtor, ela servirá para que tenhamos disponível de maneira automática todas as versões que temos na nossa API pois nela temos uma lista das mesmas.
{doc07}
No primeiro método que vemos ele recebe dois parâmetros, não iremos utilizar ele, então apenas preencheremos chamando o outro método com mesmo nome que recebe apenas um e passaremos o atributo de opções que será o necessário para essa configuração.
{doc07-1}
Logo no início de nosso segundo método temos um laço de repetição que irá adicionar um novo arquivo Swagger para cada versão disponível em nossa API, passando o número da sua versão e chamando nossa função de criar informações sobre o arquivo que temos no final da classe.
{doc08}
Em seguida temos a configuração para que o Swagger consiga encontrar os arquivos XML das documentações que fizemos em nossas entidades, junto com as linhas que adicionamos em nosso csproj que completa essa configuração.
{doc09}
Ao ligarmos as 'annotations' estamos liberando para que o swagger acesse também outra configuração que faremos diretamente em nossos controllers que irão lidar com o nome dos métodos, status codes e tipos de retornos produzidos.
{doc10}
Esse trecho não é obrigatório, ele lida com a parte de adicionar algum recurso de autorização para acesso aos nossos endpoints, caso seja uma API de estudos que não estamos lidando com nenhum tipo de token de validação é só não inserir que o swagger funcionará normalmente. Essa simulação é utilizando as definições de um token que é o que mais vemos sendo usado no mercado o JWT(Json Web Tokens. <url>)
{doc11}
Chegando ao final dessa parte da classe, temos o método interno que adiciona as informações sobre a API, nome, descrição, termos, contato e etc, com o detalhe adicional na informação de API depreciada, como estamos recebendo para criação dessas informações uma ApiVersionDescription ela contém dentro além da versão também o status de depreciada ou não, com isso podemos adicionar uma condicional e na descrição da API informar nosso consumidor que a mesma não é a mais atual e que ele deve procurar se adequar a uma mais recente.
{doc12}

Chegando no fim da nossa configuração do Swagger temos mais uma classe para criar, essa será uma classe de extensão que será responsável por adicionar esse swagger à execução do nosso sistema.

{doc13}

A primeira configuração que fazemos é bem simples, adicionando aos services de nossa aplicação a SwaggerGen que é padrão e para configurar passamos nossa classe criada no passo acima, o framework toma conta do restante.

O próximo passo é um pouco mais longo porém bem simples também, vamos adicionar o SwaggerUI a nosso app, primeiro buscamos no nosso service o nosso provider de versões e armazemos em uma variável para ser usado logo mais abaixo, após isso vamos dizer para o app usar o swagger e em último lugar faremos um laço de repetição dentro da configuração para usar o SwaggerUI, informando onde ele deve buscar os arquivos JSON que serão usados para renderizar cada versão. Uma pequena nota a respeito desse passo é que utilizamos o método .Reverse() dentro da definição do foreach isso serve para que a versão mais recente seja a prioritária ao abrir o swagger.

Esse passo a passo foi um pouco mais longo, mas já estamos chegando no final do artigo, então vamos só mais um detalhe muito importante que é como documentar nossos métodos dentro das controllers.

### Documentando nossas controllers

Existem duas formas de utilizar o pacote de versionamento das controllers, uma delas permite que façamos tudo dentro da mesma classe, mantendo ambas as versões disponíveis e temos que indicar método a método qual das duas versões atenderia, pode fazer sentido em certos cenários porém acho mais interessante usar a abordagem de segmentar em classes e pastas diferentes, assim ficamos com uma melhor organização e facilita o trabalho de refatoração quando necessário com classes mais concisas a estrutura que vamos usar de diretórios é conforme na imagem abaixo:

{doc14}

Dessa forma temos diretórios para cada versão de nossa API, após isso vamos olhar as annotations usadas dentro de cada controller e em seguida acima de cada método.

{d0c15}

Algumas coisas mantém com o que vemos na maioria dos tutoriais e cursos, anotar com o Authorize pra quando houver alguma ferramenta de antenticação, como o JWT, a rota com um pequeno detalhe sobre a versão que precisamos adotar quando estamos trabalhando com o pacote de versionamento, junto com a a anotação que define a versão da api e nesse caso definindo como depreciada e isso será usado na nossa classe de confiuguração mostrada anteriormente para adicionar um texto informativo tal qual para adicionar no header response das chamadas HTTP informando seu status.
As anotações de Produces e Consumes são opcionais e também podem ser usadas método a método dependendo de sua necessidade, porém como padrão dessa nossa API trabalha apenas com entrada e saída de JSON, definindo no topo da classe ela se reflete em todos os métodos.

{doc16}

Na nossa controller V2 só muda que não temos a anotação confirmando que a API está depreciada nessa versão, seguindo restante completamente igual.

### Documentando nosso CRUD

Agora como fazemos para documentar cada um dos nossos métodos? Seguem abaixo exemplo de todos os métodos que temos na nossa API.

#### Create

{doc17}

A primeira anotação que vemos é a que diz qual o método HTTP que está sendo usado, seguindo o estilo arquitetural REST para criação de novos recursos utilizamos o POST e como não estamos criando em um sub recurso específico, entendemos que estamos criando um novo registro do recurso da nossa controller, no caso Aluno.
As anotações de SwaggerResponse definem as respostas possíveis ao realizar a chamada da nossa api, temos que ter uma anotação para cada tipo possível que iremos retornar, nesse caso podemos usar um 201 Created ou 400 BadRequest, porém isso depende de cada método, logo a frente na mesma anotação definimos o tipo de retorno, se será uma entidade, modelo de resposta com erro ou qualquer que seja o que nossa API irá retornar, fazendo isso garantimos que nosso swagger fique completo e, como nossas entidades estão também documentadas, trazendo também exemplos que definimos para cada campo do modelo.
Na anotação SwaggerOperation teremos o nome da chamada (summary) e a sua descrição (description) e essas strings serão usadas em nosso swagger, conforme exemplo abaixo da UI final.

{doc18}

É um processo relativamente longo e verboso, mas o resultado definitivamente vale a pena, ficamos com essa documentação super completa e que é fácil de ser mantida e acompanhar as mudanças do código, por estar altamente atrelada a ele.

Vou deixar o exemplo dos outros métodos como referência, mas isso vai ser sempre para atender o cenário de cada API, então varia bastante para atender a sua necessidade.

#### Read

{doc19}

#### Update

{doc20}

#### Delete

{doc21}

E com isso chegamos ao fim do que é uma forma muito boa de documentar nossas APIs, facilitando o consumo e atualização de nossos endpoints, lembrando que podemos fazer isso junto com uma abordagem de API first, pois não precisamos de nenhuma regra de negócio, apenas com os controllers e configurações já temos o swagger para disponibilizar para quem interessar.
O código completo está nesse reposótirio do Github, se chegou até aqui muito obrigado!