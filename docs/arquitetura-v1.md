# Arquitetura Tecnica V1

## Objetivo

Definir a arquitetura inicial do `comandaai` para a V1 com foco em:

- app cliente PWA mobile-first
- operacao conectada ao ambiente local do estabelecimento
- painel interno e painel publico no mesmo ecossistema
- baixo atrito operacional
- evolucao futura para componentes mais desacoplados sem reescrita do dominio

## Decisao arquitetural

A V1 sera desenhada em quatro limites principais:

1. `App Cliente PWA`
2. `Backend/API Local ou Hibrida`
3. `Painel Interno Operacional`
4. `Painel Publico de Pedidos`

O repositorio atual inicia pela frente `App Cliente PWA`, que reaproveita o ecossistema `.NET 10` com Blazor WebAssembly e suporte a PWA.

## Limites da V1

### 1. App Cliente PWA

Responsabilidades:

- leitura de QR code e entrada no fluxo do estabelecimento
- exibicao do cardapio por categoria
- selecao de adicionais, observacoes e itens
- identificacao por nome ou apelido
- envio do pedido
- acompanhamento do status do pedido

O que nao deve concentrar:

- regra de fila operacional multiusuario
- controle definitivo do painel
- autenticacao administrativa
- persistencia central do negocio

### 2. Backend/API Local ou Hibrida

Responsabilidades:

- validar contexto operacional do estabelecimento
- receber pedidos do cliente
- gerar numero do pedido
- manter fila e status do pedido
- propagar atualizacoes em tempo real para paineis e cliente

Diretriz:

- essa camada deve nascer separada do app cliente PWA, mesmo que a primeira versao seja simples ou hospedada localmente
- no estado atual do repositorio, o envio ainda usa um gateway local simulado na camada `Application`, preservando o contrato para futura troca por integracao real

### 3. Painel Interno Operacional

Responsabilidades:

- exibir pedidos recebidos
- priorizar leitura e toque rapido
- atualizar status de atendimento
- refletir fila em tempo real

### 4. Painel Publico de Pedidos

Responsabilidades:

- exibir numero e nome/apelido
- destacar pedidos prontos
- priorizar leitura a distancia

## Organizacao interna do app cliente PWA

O app cliente da V1 segue estes limites internos:

- `Components`: UI em Razor Components e layouts
- `Application`: contratos de servico e orquestracao do app
- `Domain`: entidades e regras centrais do negocio
- `Persistencia local`: estado temporario do cliente no navegador para carrinho e continuidade do fluxo
- `wwwroot`: ativos visuais, manifesto PWA e service worker

## Modelagem inicial do dominio

### Cardapio

- `CategoriaCardapio`
- `ProdutoCardapio`
- `AdicionalCardapio`
- `ProdutoCardapioTipo`

### Pedido

- `Pedido`
- `ItemPedido`
- `ItemPedidoAdicional`
- `PedidoStatus`
- `IdentificacaoCliente`
- `ReferenciaAtendimento`

## Fluxo principal da V1

1. Cliente acessa o app PWA pelo QR code do estabelecimento.
2. App valida contexto operacional permitido.
3. Cliente informa nome ou apelido.
4. Cliente monta o pedido no cardapio, com observacoes, adicionais e carrinho local persistido no navegador.
5. App monta o aggregate de pedido pela camada `Application` antes do envio real.
6. Pedido e enviado ao backend local/hibrido.
7. Backend gera numero e status inicial.
8. Painel interno recebe o pedido em tempo real.
9. Operacao atualiza status.
10. Painel publico e cliente refletem a evolucao do pedido.

## Seguranca e privacidade

- nao confiar apenas em SSID como criterio de autorizacao
- minimizar dados pessoais no fluxo do cliente
- tratar QR code, apelido, observacoes e payloads como entrada nao confiavel
- evitar vazar detalhes internos de topologia local em mensagens de erro

## Escalabilidade

A V1 nao precisa nascer em microsservicos, mas o dominio deve permitir separacao futura de:

- catalogo
- pedidos
- fila operacional
- exibicao de painel

Essa separacao deve acontecer por contratos e limites claros, nao por distribuicao prematura.

## Fluxo de entrega automatizado

- cada round deve nascer em uma branch nova
- `main` e `development` ficam protegidas contra commit direto e aceitam mudancas apenas por pull request
- todo push e toda abertura de pull request disparam a pipeline `Build`
- o termo `final branch commit` fecha uma round branch e dispara build, teste quando houver projeto de teste, criacao automatica de pull request para `development`, auto-merge, confirmacao de merge e publicacao da prerelease sequencial
- a prerelease automatica deve ser publicada com titulo enxuto no formato `Prerelease 0.0.x`

## Estado atual do repositorio

- app web PWA inicial criado em `src/web/app`
- versao de trabalho inicial definida como `0.0.1`
- validacao local ainda pode continuar sem build e sem testes obrigatorios
- pipelines configuradas para build em push e PR, round final automatizada e prerelease apos merge em `development`
- a publicacao de prerelease foi acoplada ao fechamento automatizado do round para evitar lacuna causada pelo merge executado por workflow
- primeiro projeto de testes criado em `tests/ComandaAi.Web.Tests` com foco em dominio e camada `Application`
- round 2 consolidou carrinho customizavel com persistencia local no navegador e preview de pedido montado pela camada `Application`
- round 4 consolidou o contrato de envio do pedido com gateway local simulado e confirmacao operacional na UI