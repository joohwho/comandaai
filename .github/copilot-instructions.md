# Project Guidelines

## Contexto do produto

O `comandaai` e um app de comanda digital para bares e restaurantes.

Direcao atual do produto:

- PWA mobile-first
- .NET 10
- Blazor WebAssembly com PWA
- Blazor com Razor Components
- acesso inicial por QR code e browser
- operacao conectada ao ambiente local do estabelecimento
- painel interno e painel publico como partes do mesmo ecossistema

## Fluxo de entrega e governanca de branches

- O trabalho passa a acontecer por rounds, com uma nova branch a cada round.
- `main` e `development` devem permanecer protegidas e sem commit direto.
- Merge em `main` e `development` deve acontecer apenas via pull request.
- O termo `final branch commit` identifica o commit final de uma round branch.
- Ao encontrar `final branch commit`, a automacao deve considerar build obrigatorio, teste quando houver projeto de teste, criacao automatica de pull request para `development` e auto-merge.
- Cada merge em `development` deve gerar prerelease sequencial no formato `0.0.x`.

## Qualidade de codigo

- Aplicar Clean Code, SOLID e POO com foco pragmatico.
- Manter classes, servicos e componentes pequenos, coesos e com responsabilidade clara.
- Favorecer nomes explicitos, contratos claros e fluxos previsiveis.
- Evitar duplicacao acidental e abstracoes prematuras.
- Isolar regra de negocio de detalhes de plataforma, UI e infraestrutura sempre que possivel.

## Arquitetura

- Reaproveitar o padrao estrutural dos projetos `qa-test-pilot` e `inovar-colors-orcamento` quando ele fizer sentido para o novo dominio.
- Separar claramente camadas de UI, aplicacao, dominio, persistencia local no browser e integracoes.
- Tratar backend, realtime, fila operacional e paineis como limites arquiteturais explicitos.
- Preparar o dominio para evolucao futura em direcao a servicos desacoplados, sem forcar microsservicos cedo demais.
- Nao misturar decisao de PWA mobile-first com restricoes do backend ou do painel.

## C# .NET

- Usar boas praticas modernas de C# .NET com null safety, DI, composicao e contratos explicitos.
- Preferir tipos pequenos, records e services quando combinarem com o problema.
- Evitar classes deus, metodos longos e estados implicitos dificeis de validar.
- Modelar o dominio com foco em categorias, produtos, adicionais, pedidos, itens, status, mesa/comanda e operadores internos.
- Em componentes Razor, manter paginas pequenas e empurrar regra para services e dominio.

## Seguranca e privacidade

- Tratar toda entrada como nao confiavel.
- Minimizar coleta, persistencia e exibicao de dados pessoais.
- Nao confiar apenas em SSID para validar uso local.
- Evitar vazar detalhes internos em logs, mensagens ou respostas de erro.
- Explicitar riscos quando uma decisao afetar privacidade, seguranca operacional ou governanca.

## Governanca e transparencia

- Explicitar trade-offs relevantes em decisoes arquiteturais.
- Registrar decisoes importantes em documentacao quando elas afetarem evolucao futura.
- Manter comportamento do sistema compreensivel para operacao interna e cliente final.
- Priorizar mensagens claras de status, erro e indisponibilidade.

## UX, responsividade e acessibilidade

- Priorizar fluxos rapidos e claros para ambiente de bar.
- Garantir boa experiencia em celular no app cliente PWA e leitura eficiente nos paineis.
- Considerar contraste, tamanho de toque, foco, semantica e feedback visual como requisitos reais.
- Evitar interfaces que dependam de precisao excessiva, excesso de texto ou ambiguidades de estado.

## Implementacao e validacao

- Preferir mudancas pequenas e validaveis.
- Antes de adicionar bibliotecas, confirmar que a plataforma base nao resolve o problema de forma mais simples.
- Enquanto a suite de testes ainda estiver inicial, aceitar pipeline com etapa de teste condicional para os casos em que ainda nao houver cobertura no modulo tocado.
- Nessa fase, validacao local continua podendo usar consistencia estrutural, coerencia arquitetural, leitura de codigo e criterios objetivos que nao dependam de execucao local.
- Nao introduzir backend framework, banco definitivo, autenticacao ou pagamento sem decisao explicita.