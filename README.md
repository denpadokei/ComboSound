# ComboSound

[![Build and Release](https://github.com/denpadokei/ComboSound/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/denpadokei/ComboSound/actions/workflows/build-and-release.yml)

コンボ数が決まった値に達したときに効果音を鳴らす MOD です。太鼓の達人の「コンボ達成音」のような演出を Beat Saber に追加します。効果音はフォルダ単位のプリセットとして差し替えられます。

## 機能

- コンボ数が `combo_XXXX.wav` の数字と一致したときに効果音を再生
- `UserData/ComboSound/<プリセット名>/` フォルダをプリセットとして自動読み込み
- 内蔵プリセット **Don-Chan**（50〜9000 コンボの 19 ファイル）を同梱
- ゲーム内の Mod Settings の **COMBO SOUND** タブで有効／無効、音量、プリセットを選択
- 音量は 0〜100% で設定し、再生時に `Volume / 100` の音量で再生
- プリセットを追加すると選択肢に自動で表示（ゲーム再起動後に反映）

## 動作環境

| 項目 | 内容 |
| --- | --- |
| Beat Saber | 1.40.8 |
| 依存 MOD | BSIPA 4.3.6 / BeatSaberMarkupLanguage 1.12.5 / SiraUtil 3.2.0 |

## インストール

1. Beat Saber を 1.40.8 に合わせる (BSManager などのバージョン管理ツールを推奨)
2. BSIPA と上記の依存 MOD をインストールする
3. [Releases](https://github.com/denpadokei/ComboSound/releases) から `ComboSound-4.0.0-bs1.40.8.zip` をダウンロードし、`Plugins` と `UserData` フォルダをゲームフォルダへ上書きコピーする

Mod Assistant からもインストールできます。

## 使い方

効果音は次の配置で読み込まれます。

```text
UserData/
└─ ComboSound/
   ├─ Don-Chan/
   │  ├─ combo_0050.wav
   │  ├─ combo_0100.wav
   │  └─ ...
   └─ <プリセット名>/
      ├─ combo_0100.wav
      └─ ...
```

- フォルダ名がそのままプリセット名になります。
- ファイル名は `combo_(数字4桁).wav` にしてください。数字がコンボ数です（例: `combo_0100.wav` は 100 コンボで再生）。
- 同じコンボ数のファイルが複数ある場合は、最初の 1 つだけが読み込まれます。
- WAV 以外の形式や、4 桁でない数字のファイルは無視されます。
- ゲーム内の **COMBO SOUND** タブでプリセットと音量を選べます。変更は次のプレイから反映されます。

## 設定

設定は `UserData/ComboSound.json` に保存されます。

| 項目 | 型 | 既定値 | 説明 |
| --- | --- | --- | --- |
| Enable | bool | true | コンボ効果音を有効にする |
| Volume | int | 100 | 再生音量（0〜100） |
| CurrentSound | string | "" | 使用するプリセットのフォルダ名。空の場合は最初に見つかったプリセット |

## ビルド

```powershell
git clone https://github.com/denpadokei/ComboSound.git
cd ComboSound
dotnet build .\ComboSound\ComboSound.csproj -c Release
```

`BeatSaberDir` に Beat Saber のインストール先を設定すると、ゲームの参照アセンブリを解決できます。

GitHub Actions の `build-and-release.yml` が push ごとにビルドし、`X.X.X` 形式のタグを push するとリリースを作成します。

## ライセンス

MIT License（`LICENSE`）。
