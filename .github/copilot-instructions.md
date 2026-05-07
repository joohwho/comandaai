# Project Guidelines

## Contexto do produto

O `comandaai` e um app de comanda digital para bares e restaurantes.

Direcao atual do produto:

- Android-first
- .NET 10
- .NET MAUI Blazor Hybrid
- Blazor com Razor Components
- distribuicao inicial por APK e QR code
- operacao conectada ao ambiente local do estabelecimento
- painel interno e painel publico como partes do mesmo ecossistema

## Qualidade de codigo

- Aplicar Clean Code, SOLID e POO com foco pragmatico.
- Manter classes, servicos e componentes pequenos, coesos e com responsabilidade clara.
- Favorecer nomes explicitos, contratos claros e fluxos previsiveis.
- Evitar duplicacao acidental e abstracoes prematuras.
- Isolar regra de negocio de detalhes de plataforma, UI e infraestrutura sempre que possivel.

## Arquitetura

- Reaproveitar o padrao estrutural dos projetos `qa-test-pilot` e `inovar-colors-orcamento` quando ele fizer sentido para o novo dominio.
- Separar claramente camadas de UI, aplicacao, dominio, persistencia local e integracoes.
- Tratar backend, realtime, fila operacional e paineis como limites arquiteturais explicitos.
- Preparar o dominio para evolucao futura em direcao a servicos desacoplados, sem forcar microsservicos cedo demais.
- Nao misturar decisao de distribuicao Android-first com restricoes do backend ou do painel.

## C# .NET

- Usar boas praticas modernas de C# .NET com null safety, DI, composicao e contratos explicitos.
- Preferir tipos pequenos, records e services quando combinarem com o problema.
- Evitar classes deus, metodos longos e estados implicitos dificeis de validar.
- Modelar o dominio com foco em categorias, produtos, adicionais, pedidos, itens, status, mesa/comanda e operadores internos.

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
- Garantir boa experiencia em celular no app cliente e leitura eficiente nos paineis.
- Considerar contraste, tamanho de toque, foco, semantica e feedback visual como requisitos reais.
- Evitar interfaces que dependam de precisao excessiva, excesso de texto ou ambiguidades de estado.

## Implementacao e validacao

- Preferir mudancas pequenas e validaveis.
- Antes de adicionar bibliotecas, confirmar que a plataforma base nao resolve o problema de forma mais simples.
- Sempre que possivel, validar por teste, build, checagem local ou criterio executavel equivalente.
- Nao introduzir backend framework, banco definitivo, autenticacao ou pagamento sem decisao explicita.