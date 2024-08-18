const qstring = {
  get: (name) => {
    const search = window.location.search;
    const params = new URLSearchParams(search);
    const val = params.get(name);
    return val;
  },
};

export default qstring;
