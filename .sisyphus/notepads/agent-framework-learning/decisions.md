# Decisions - Agent Framework Learning Plan

## 2026-05-18

### Learning Mode
- User chose "逐步引导模式" (step-by-step guidance)
- User writes code themselves, I provide concepts, patterns, and verification
- NOT code-generation mode

### Security Decision
- appsettings.json must be gitignored
- appsettings.template.json created as placeholder
- Real API key only stored locally