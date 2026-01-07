# 4711

这是一个 Windows 10/11 的桌面浮层工具（Clicknow-like）项目骨架。

## 运行环境
- Windows 10/11
- .NET 8 SDK

## 启动说明（MVP 骨架）
1. 在 Windows 上打开终端并进入项目目录：
   ```bash
   cd src/ClicknowOverlay
   ```
2. 运行：
   ```bash
   dotnet run
   ```

> 当前仅包含一个最小 WPF 窗口，用于验证 UI、透明窗口与按钮布局。

## 下一步（建议）
- 接入全局热键与选区捕获
- 接入 ClipboardManager（复制/恢复）
- 实现 SearchProvider / LLMProvider / TranslateService
- 将结果面板与真实逻辑接通
