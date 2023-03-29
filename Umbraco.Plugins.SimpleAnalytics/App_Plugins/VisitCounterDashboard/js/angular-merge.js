if (!angular.merge) {
    angular.merge = (function mergePollyfill() {
        function isObject(value) {
            return value !== null && typeof value === 'object';
        }
        function isDate(value) {
            return toString.call(value) === '[object Date]';
        }
        function isRegExp(value) {
            return toString.call(value) === '[object RegExp]';
        }
        function isElement(node) {
            return !!(node &&
                (node.nodeName  // We are a direct element.
                    || (node.prop && node.attr && node.find)));  // We have an on and find method part of jQuery API.
        }
        function isArray(arr) {
            return Array.isArray(arr) || arr instanceof Array;
        }
        function isArrayLike(obj) {

            // `null`, `undefined` and `window` are not array-like
            if (obj == null || isWindow(obj)) return false;

            // arrays, strings and jQuery/jqLite objects are array like
            // * jqLite is either the jQuery or jqLite constructor function
            // * we have to check the existence of jqLite first as this method is called
            //   via the forEach method when constructing the jqLite object in the first place
            if (isArray(obj) || isString(obj) || (jqLite && obj instanceof jqLite)) return true;

            // Support: iOS 8.2 (not reproducible in simulator)
            // "length" in obj used to prevent JIT error (gh-11508)
            var length = 'length' in Object(obj) && obj.length;

            // NodeList objects (with `item` method) and
            // other objects with suitable length characteristics are array-like
            return isNumber(length) && (length >= 0 && (length - 1) in obj || typeof obj.item === 'function');
        }

        function setHashKey(obj, h) {
            if (h) {
                obj.$$hashKey = h;
            } else {
                delete obj.$$hashKey;
            }
        }

        function baseExtend(dst, objs, deep) {
            var h = dst.$$hashKey;

            for (var i = 0, ii = objs.length; i < ii; ++i) {
                var obj = objs[i];
                if (!isObject(obj) && !isFunction(obj)) continue;
                var keys = Object.keys(obj);
                for (var j = 0, jj = keys.length; j < jj; j++) {
                    var key = keys[j];
                    var src = obj[key];

                    if (deep && isObject(src)) {
                        if (isDate(src)) {
                            dst[key] = new Date(src.valueOf());
                        } else if (isRegExp(src)) {
                            dst[key] = new RegExp(src);
                        } else if (src.nodeName) {
                            dst[key] = src.cloneNode(true);
                        } else if (isElement(src)) {
                            dst[key] = src.clone();
                        } else {
                            if (key !== '__proto__') {
                                if (!isObject(dst[key])) dst[key] = isArray(src) ? [] : {};
                                baseExtend(dst[key], [src], true);
                            }
                        }
                    } else {
                        dst[key] = src;
                    }
                }
            }

            setHashKey(dst, h);
            return dst;
        }

        return function merge(dst) {
            return baseExtend(dst, [].slice.call(arguments, 1), true);
        }
    })();
}