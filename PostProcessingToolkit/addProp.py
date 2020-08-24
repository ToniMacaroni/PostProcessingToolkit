import sys

propType = sys.argv[1]
propName = sys.argv[2]

def getFileContent(filename):
    file = open(filename, "r")
    content = file.read()
    file.close()
    return content

def setFileContent(filename, content):
    file = open(filename, "w")
    file.write(content)
    file.close()

# === PluginConfig.cs ===
content = getFileContent("PluginConfig.cs")
content = content.replace("//{Prop}", "public "+propType+" "+propName+" { get; set; } = 0f;\n\n\t\t//{Prop}")
setFileContent("PluginConfig.cs", content)

# === bsml ===
filename =   "resources\\setup.bsml"
replaceTag = "<!-- {Prop} -->"
content = getFileContent(filename)
content = content.replace(replaceTag, f"<slider-setting text='{propName}' value='{propName}' min='0' max='1' apply-on-change='true'/>"+"\n\t\t"+replaceTag)
setFileContent(filename, content)

# === SetupUI.cs ===
if propType == "float":
    filename =   "SetupUI.cs"
    replaceTag = "//{Prop}"
    content = getFileContent(filename)
    content = content.replace(replaceTag, f'''[UIValue(nameof({propName}))]
        public {propType} {propName}''' + "\n\t\t{" + "\n\t\t\tget => PluginConfig.Instance." + propName + ";\n\t\t\tset\n\t\t\t{\n\t\t\t\tPluginConfig.Instance."+propName+" = value;\n\t\t\t\tPostProcessLoader.Instance."+propName+".Value = value;\n\t\t\t}\n\t\t}\n\n\t\t"+replaceTag)
    setFileContent(filename, content)

# === PostProcessLoader.cs ===
filename =   "PostProcessLoader.cs"
replaceTag = "//{Prop}"
replaceTag2 = "//{Prop definition}"
replaceTag3 = "//{Prop config}"
content = getFileContent(filename)
content = content.replace(replaceTag, f"public FloatMatProp {propName};\n\t\t{replaceTag}")
content = content.replace(replaceTag2, f"{propName} = new FloatMatProp(Material, nameof({propName}));\n\t\t\t{replaceTag2}")
content = content.replace(replaceTag3, f"{propName}.Value = PluginConfig.Instance.{propName};\n\t\t\t{replaceTag3}")
setFileContent(filename, content)