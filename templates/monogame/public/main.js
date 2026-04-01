import hlslLanguage from './hlsl.js';

export default {
  defaultTheme: 'dark',
  configureHljs: (hljs) => {
    hljs.registerLanguage('hlsl', hlslLanguage);
  }
}
