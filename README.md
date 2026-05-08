# comandaai

Aplicativo de comanda digital para bares e restaurantes, com foco em autoatendimento local via Wi-Fi do estabelecimento, painel de pedidos em tempo real e operação simples para cliente, caixa e cozinha.

## Visao do produto

O `comandaai` sera usado pelo cliente dentro do estabelecimento para:

- acessar o cardapio por QR code
- navegar por categorias como bebidas, doses, drinks, porcoes, lanches e pratos
- montar e enviar pedidos com nome ou apelido
- acompanhar o numero e o status do pedido

Ao mesmo tempo, a equipe interna tera um painel operacional para:

- visualizar novos pedidos em tempo real
- ordenar atendimento por fila
- atualizar status como `recebido`, `em preparo`, `pronto` e `entregue`
- exibir um painel publico estilo retirada

## Stack de referencia

Com base nos repositorios `qa-test-pilot` e `inovar-colors-orcamento`, o `comandaai` reaproveita principalmente o ecossistema .NET e a organizacao em Razor Components, mas agora com uma frente web PWA:

- .NET 10
- Blazor WebAssembly com suporte a PWA
- Blazor com Razor Components para a interface
- arquitetura orientada a servicos com DI
- persistencia local no navegador quando fizer sentido
- xUnit para testes automatizados
- GitHub Actions para CI e publicacao futura

## Baseline atual do repositorio

- versao de trabalho inicial: `0.0.1`
- scaffold web PWA inicial em `src/web/app`
- solucao principal em `comandaai.slnx`
- primeiro projeto de testes em `tests/ComandaAi.Web.Tests`
- camada `Application` ja cobre consulta de catalogo, montagem de pedido e carrinho local persistido no navegador
- agente customizado do projeto em `.github/agents/Commander.agent.md`
- documentacao de arquitetura da V1 em `docs/arquitetura-v1.md`
- pipelines em `.github/workflows`

## Fluxo de branches e rounds

- cada round de trabalho deve acontecer em uma branch nova
- `main` e `development` passam a ser branches protegidas, sem commit direto, com merge somente via pull request
- qualquer commit deve disparar pipeline de build
- ao abrir pull request, o build deve rodar novamente
- o termo exato `final branch commit` marca o commit final de um round
- quando esse termo aparecer fora de `main` e `development`, a pipeline do round executa build, executa teste quando houver projeto de teste, abre pull request para `development`, habilita auto-merge, confirma o merge e publica a prerelease sequencial
- a prerelease automatica deve usar titulo enxuto no formato `Prerelease 0.0.x`

## Direcao tecnica inicial

Partindo da stack de referencia, o projeto nasce com estas decisoes iniciais:

- app cliente em formato PWA, compativel com Android e iPhone via browser
- acesso por QR code sem depender de Play Store ou App Store
- interface mobile-first otimizada para uso no salao
- painel interno e painel publico como modulos web do mesmo ecossistema
- backend/API local ou hibrida como nova camada arquitetural do produto
- acesso funcional restrito a infraestrutura local do estabelecimento, sem depender apenas de SSID

## O que sera reaproveitado da stack anterior

- organizacao da UI em componentes Razor
- padrao de `Services` para regras de negocio e fluxos
- uso consistente de injecao de dependencia
- modelagem em C# .NET com foco em contratos claros
- padrao de testes unitarios para regras e servicos desacoplados da plataforma

## Estado do round 1

- round 1 consolidou a migracao para PWA
- pipelines de build, round final e prerelease foram configuradas
- `main` e `development` ficaram protegidas para fluxo via pull request
- a suite inicial de testes xUnit foi criada para ativar a etapa de `test` quando houver `final branch commit`

## Estado do round 2

- round 2 consolidou o primeiro fluxo cliente de pedido dentro da PWA
- o catalogo agora permite montar itens com observacoes e adicionais
- o carrinho local foi introduzido como estado do cliente com identidade por linha personalizada
- o preview de pedido passou a nascer da camada `Application`, em vez de montagem manual na pagina
- o carrinho passou a persistir em `localStorage`, preservando a selecao ao recarregar a aplicacao
- a cobertura de testes foi expandida para servicos de carrinho e montagem de pedido
- a primeira prerelease `0.0.1` foi publicada apos o merge do round em `development`

## O que nao existe ainda nos projetos anteriores

- painel de pedidos em tempo real multiusuario
- backend para fila de pedidos e sincronizacao
- estrategia de descoberta e validacao de rede local do estabelecimento
- fluxo cliente-operacao-publico no mesmo dominio de negocio

Esses pontos devem ser tratados como arquitetura nova do `comandaai`, mesmo com reaproveitamento da base mobile anterior.

## Requisitos principais da V1

1. Acesso ao app por QR code.
2. Funcionamento operacional restrito a rede do estabelecimento.
3. Cardapio organizado por categorias.
4. Itens com adicionais, observacoes e variacoes quando aplicavel.
5. Carrinho e confirmacao de pedido.
6. Identificacao do cliente por nome ou apelido.
7. Geracao automatica de numero do pedido.
8. Painel interno com fila em tempo real.
9. Painel publico de acompanhamento de pedidos.
10. Atualizacao de status pela equipe.

## Perfis e fluxos

### Cliente

1. Escaneia o QR code.
2. Abre o app PWA no navegador do celular.
3. Informa nome ou apelido.
4. Escolhe itens do cardapio.
5. Envia o pedido.
6. Acompanha o status pelo numero do pedido.

### Operacao interna

1. Recebe o pedido no painel.
2. Confirma atendimento na fila.
3. Atualiza o status do pedido conforme preparo.
4. Exibe o pedido como pronto no painel publico.
5. Finaliza como entregue.

## Regras de negocio iniciais

- O app nao deve operar normalmente fora da rede do estabelecimento.
- O cliente pode ser identificado por nome, apelido ou outro identificador simples definido pela operacao.
- Cada pedido deve receber um numero unico legivel para exibicao no painel.
- O painel deve priorizar leitura rapida e atualizacao em tempo real.
- O sistema deve nascer preparado para suportar QR code por mesa ou por comanda, mesmo que isso fique para uma versao posterior.

## Fora de escopo da V1

- integracao com meios de pagamento no app
- impressao automatica
- multiunidade
- programa de fidelidade
- analytics avancado
- operacao offline completa

## Decisoes em aberto

- forma de validacao de permanencia na rede local
- modelo de deploy local ou hibrido
- composicao do painel interno e do painel publico
- login interno da equipe
- estrategia de cadastro e edicao do cardapio
- estrategia de cache offline parcial no PWA alem da persistencia local do carrinho

## Proximos passos recomendados

1. Definir a arquitetura do backend local/hibrido e do realtime.
2. Estruturar o contrato de envio do pedido do cliente para o backend local/hibrido.
3. Decidir como o painel interno sera entregue na V1.
4. Conectar a fila operacional a uma fonte realtime real.
5. Expandir a cobertura de testes para persistencia local e fluxos integrados entre UI e Application.