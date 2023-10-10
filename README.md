# SimpleProtoc
Google Proto Buffer에서 프로토 메시지를 생성하는 프로그램(protoc)을 간단하게 실행하는 프로그램입니다. 
</br></br></br></br></br>

## Target Framework
**.NET 5.0**

## How to Use
1. SimpleProtoc.bat 파일을 실행하거나 SimpleProtoc.exe 파일을 실행합니다.
<br>(args.json 파일이 실행 경로에 존재하지 않을 경우, 새로 파일을 생성하고 종료됩니다.)
1. 출력 경로에 생성된 파일을 확인할 수 있습니다.

## args.json
protoc에 대한 실행 인자 값을 지정하는 설정 파일입니다.
### Fields
* PROTOC_PATH: 클래스 생성하는 실행파일의 경로 (빈 칸일시에 default적용), Default: `protoc.exe`
* IMPORT_PATH: proto파일을 탐색할 디렉터리 경로, Default: `present directory`
* FILES: 클래스를 생성할 proto 파일들(list), TARGET_DIR 값이 존재하면 무시되고 1개 이상 반드시 지정 필요
* TARGET_DIR: 클래스를 생성할 proto 파일들이 있는 디렉터리 경로, 해당 디렉터리에 존재하는 모든 proto 파일들을 생성
* COMMON_OUTPUT_PATH: 생성된 파일들이 위치할 공통 디렉터리 경로, 언어에 특정 경로를 지정하지 않으면 이 값을 사용, Default: `current directory`
* LANGS: 클래스를 생성할 언어, 생성할 언어는 `true`로 그렇지 않으면 `false`
* LANGS_PATH: LANGS가 true로 설정된 언어들에 한해, 생성된 클래스 파일의 경로를 각각 설정, Default: `COMMON_OUTPUT_PATH`

## Notes
* 값으로 지정하는 path는 존재해야 합니다. (프로그램에서 새로운 디렉터리를 생성하지 않습니다.)
* Go 언어와 Darts 언어는 지원하지 않습니다.
* namespace나 package의 경로 등의 옵션은 proto 파일에 직접 작성해야 합니다.
* 실행 인자는 별로로 존재하지 않습니다.
* args.json 파일이 없을 경우, 프로그램 첫 실행 시에 생성됩니다.
* 디렉터리 구분 문자는 `/`를 사용합니다.
* `.proto`확장자에 대한 프로토 파일만 인식합니다.
* 클래스 생성은 `protoc.exe`에서 실행합니다. 해당 파일 실행 오류 내용은 표시됩니다.
* SimpleProtoc.bat 파일을 실행 시키기 위해서는 `.dll` 파일과 `.runtimeconfig.json`파일이 같은 경로에 위치해 있어야 합니다.
* SimpleProtoc.bat 파일의 내용은 다음과 같습니다.
```
@echo off
SimpleProtoc.exe
pause
```

## Languages
* C++
* Java
* Python(Pyi)
* Ruby
* Objective-C
* C#
* Kotlin
* PHP

##### (Go와 Darts의 클래스를 생성하기 위해서는 별도의 모듈 설치가 필요합니다.)

## Google Protobuf Page
[Google Protocol Buffers Documentation](https://protobuf.dev/)

