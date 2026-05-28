/** @type {import('ts-jest').JestConfigWithTsJest} */
module.exports = {
  preset: "ts-jest",
  // office-addin-mock använder Node-API:er internt, så testmiljön måste
  // vara "node" (inte "jsdom"). I ett task pane-projekt med både React-tester
  // och Office-tester kan man istället sätta miljön per fil med en docblock:
  //   /** @jest-environment node */
  testEnvironment: "node",
};
