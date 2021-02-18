export function isListener(attrs: { [key: string]: unknown }, attrKey: string) {
  return attrKey.startsWith("on") && typeof attrs[attrKey] === "function";
}

export function getListeners(attrs: { [key: string]: unknown }) {
  return Object.keys(attrs).reduce((prev, key) => {
    if (isListener(attrs, key)) {
      prev[key] = attrs[key];
    }

    return prev;
  }, {} as { [key: string]: any });
}
