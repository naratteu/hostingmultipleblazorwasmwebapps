# 여러개의 BlazorWebassembly 독립실행형 애플리케이션을 서브디렉토리에 같이 호스팅

## Deployment

```bash
dotnet new install Microsoft.FluentUI.AspNetCore.Templates::4.11.7

for proj in blazorwasm fluentblazorwasm; do
    dotnet new $proj -o $proj
    sed -i "/<PropertyGroup>/a\    <StaticWebAssetBasePath>/$proj/</StaticWebAssetBasePath>" ./$proj/$proj.csproj
    sed -i "s|<base href=\"/\" />|<base href=\"/$proj/\" />|" ./$proj/wwwroot/index.html
done

dotnet publish ./ExampleHost
cd ExampleHost/bin/Release/net9.0/publish && ./ExampleHost
```

## Debug each

```bash
dotnet run --project blazorwasm --pathbase=/blazorwasm
dotnet run --project fluentblazorwasm --pathbase=/fluentblazorwasm # 경로문제 발생함.
```

## Analyze

- `<StaticWebAssetBasePath>` 를 지정하면 웹앱 진입점및 어셋들의 경로는 바뀌나, 참조할 `_content` 폴더만 루트에 모임
- 하지만 프론트상의 `_content` 경로는 루트로 고정도 아니고 `<base>`경로를 따라감.
- 디버그용 웹서버는 `<StaticWebAssetBasePath>`로 배포되는것을 무시한채 가동되고 `--pathbase` 는 `_content`의 경로를 바꿔줌
- fluentblazorwasm 의 경우
    - Home을 클릭하면 베이스경로(.)가 아닌 루트경로(/)로 이동함.
    - 일부 css 경로가 루트`/_content` 혹은 `../_content` 기준으로 되있음.
- 왜 이난리가 나있는지 모르겠음. 선언,디버그,배포 다 맛탱이가 가게됨.
- BlazorWasm 웹앱을 자유로운 경로에 삽입하는 모듈로써 활용하려면 대체 어디서부터 건드려야 할지, 아무것도 정리가 안되있어보임.
