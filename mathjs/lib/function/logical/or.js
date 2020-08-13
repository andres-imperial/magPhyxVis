"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.createOr = void 0;

var _algorithm = require("../../type/matrix/utils/algorithm03");

var _algorithm2 = require("../../type/matrix/utils/algorithm12");

var _algorithm3 = require("../../type/matrix/utils/algorithm13");

var _algorithm4 = require("../../type/matrix/utils/algorithm14");

var _algorithm5 = require("../../type/matrix/utils/algorithm05");

var _factory = require("../../utils/factory");

var _number = require("../../plain/number");

var name = 'or';
var dependencies = ['typed', 'matrix', 'equalScalar', 'DenseMatrix'];
var createOr = /* #__PURE__ */(0, _factory.factory)(name, dependencies, function (_ref) {
  var typed = _ref.typed,
      matrix = _ref.matrix,
      equalScalar = _ref.equalScalar,
      DenseMatrix = _ref.DenseMatrix;
  var algorithm03 = (0, _algorithm.createAlgorithm03)({
    typed: typed
  });
  var algorithm05 = (0, _algorithm5.createAlgorithm05)({
    typed: typed,
    equalScalar: equalScalar
  });
  var algorithm12 = (0, _algorithm2.createAlgorithm12)({
    typed: typed,
    DenseMatrix: DenseMatrix
  });
  var algorithm13 = (0, _algorithm3.createAlgorithm13)({
    typed: typed
  });
  var algorithm14 = (0, _algorithm4.createAlgorithm14)({
    typed: typed
  });
  /**
   * Logical `or`. Test if at least one value is defined with a nonzero/nonempty value.
   * For matrices, the function is evaluated element wise.
   *
   * Syntax:
   *
   *    math.or(x, y)
   *
   * Examples:
   *
   *    math.or(2, 4)   // returns true
   *
   *    a = [2, 5, 0]
   *    b = [0, 22, 0]
   *    c = 0
   *
   *    math.or(a, b)   // returns [true, true, false]
   *    math.or(b, c)   // returns [false, true, false]
   *
   * See also:
   *
   *    and, not, xor
   *
   * @param  {number | BigNumber | Complex | Unit | Array | Matrix} x First value to check
   * @param  {number | BigNumber | Complex | Unit | Array | Matrix} y Second value to check
   * @return {boolean | Array | Matrix}
   *            Returns true when one of the inputs is defined with a nonzero/nonempty value.
   */

  var or = typed(name, {
    'number, number': _number.orNumber,
    'Complex, Complex': function ComplexComplex(x, y) {
      return x.re !== 0 || x.im !== 0 || y.re !== 0 || y.im !== 0;
    },
    'BigNumber, BigNumber': function BigNumberBigNumber(x, y) {
      return !x.isZero() && !x.isNaN() || !y.isZero() && !y.isNaN();
    },
    'Unit, Unit': function UnitUnit(x, y) {
      return or(x.value || 0, y.value || 0);
    },
    'SparseMatrix, SparseMatrix': function SparseMatrixSparseMatrix(x, y) {
      return algorithm05(x, y, or);
    },
    'SparseMatrix, DenseMatrix': function SparseMatrixDenseMatrix(x, y) {
      return algorithm03(y, x, or, true);
    },
    'DenseMatrix, SparseMatrix': function DenseMatrixSparseMatrix(x, y) {
      return algorithm03(x, y, or, false);
    },
    'DenseMatrix, DenseMatrix': function DenseMatrixDenseMatrix(x, y) {
      return algorithm13(x, y, or);
    },
    'Array, Array': function ArrayArray(x, y) {
      // use matrix implementation
      return or(matrix(x), matrix(y)).valueOf();
    },
    'Array, Matrix': function ArrayMatrix(x, y) {
      // use matrix implementation
      return or(matrix(x), y);
    },
    'Matrix, Array': function MatrixArray(x, y) {
      // use matrix implementation
      return or(x, matrix(y));
    },
    'SparseMatrix, any': function SparseMatrixAny(x, y) {
      return algorithm12(x, y, or, false);
    },
    'DenseMatrix, any': function DenseMatrixAny(x, y) {
      return algorithm14(x, y, or, false);
    },
    'any, SparseMatrix': function anySparseMatrix(x, y) {
      return algorithm12(y, x, or, true);
    },
    'any, DenseMatrix': function anyDenseMatrix(x, y) {
      return algorithm14(y, x, or, true);
    },
    'Array, any': function ArrayAny(x, y) {
      // use matrix implementation
      return algorithm14(matrix(x), y, or, false).valueOf();
    },
    'any, Array': function anyArray(x, y) {
      // use matrix implementation
      return algorithm14(matrix(y), x, or, true).valueOf();
    }
  });
  return or;
});
exports.createOr = createOr;