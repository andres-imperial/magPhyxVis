"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.createRightLogShift = void 0;

var _algorithm = require("../../type/matrix/utils/algorithm02");

var _algorithm2 = require("../../type/matrix/utils/algorithm11");

var _algorithm3 = require("../../type/matrix/utils/algorithm13");

var _algorithm4 = require("../../type/matrix/utils/algorithm14");

var _algorithm5 = require("../../type/matrix/utils/algorithm01");

var _algorithm6 = require("../../type/matrix/utils/algorithm10");

var _algorithm7 = require("../../type/matrix/utils/algorithm08");

var _factory = require("../../utils/factory");

var _number = require("../../plain/number");

var name = 'rightLogShift';
var dependencies = ['typed', 'matrix', 'equalScalar', 'zeros', 'DenseMatrix'];
var createRightLogShift = /* #__PURE__ */(0, _factory.factory)(name, dependencies, function (_ref) {
  var typed = _ref.typed,
      matrix = _ref.matrix,
      equalScalar = _ref.equalScalar,
      zeros = _ref.zeros,
      DenseMatrix = _ref.DenseMatrix;
  var algorithm01 = (0, _algorithm5.createAlgorithm01)({
    typed: typed
  });
  var algorithm02 = (0, _algorithm.createAlgorithm02)({
    typed: typed,
    equalScalar: equalScalar
  });
  var algorithm08 = (0, _algorithm7.createAlgorithm08)({
    typed: typed,
    equalScalar: equalScalar
  });
  var algorithm10 = (0, _algorithm6.createAlgorithm10)({
    typed: typed,
    DenseMatrix: DenseMatrix
  });
  var algorithm11 = (0, _algorithm2.createAlgorithm11)({
    typed: typed,
    equalScalar: equalScalar
  });
  var algorithm13 = (0, _algorithm3.createAlgorithm13)({
    typed: typed
  });
  var algorithm14 = (0, _algorithm4.createAlgorithm14)({
    typed: typed
  });
  /**
   * Bitwise right logical shift of value x by y number of bits, `x >>> y`.
   * For matrices, the function is evaluated element wise.
   * For units, the function is evaluated on the best prefix base.
   *
   * Syntax:
   *
   *    math.rightLogShift(x, y)
   *
   * Examples:
   *
   *    math.rightLogShift(4, 2)               // returns number 1
   *
   *    math.rightLogShift([16, -32, 64], 4)   // returns Array [1, 2, 3]
   *
   * See also:
   *
   *    bitAnd, bitNot, bitOr, bitXor, leftShift, rightLogShift
   *
   * @param  {number | Array | Matrix} x Value to be shifted
   * @param  {number} y Amount of shifts
   * @return {number | Array | Matrix} `x` zero-filled shifted right `y` times
   */

  var rightLogShift = typed(name, {
    'number, number': _number.rightLogShiftNumber,
    // 'BigNumber, BigNumber': ..., // TODO: implement BigNumber support for rightLogShift
    'SparseMatrix, SparseMatrix': function SparseMatrixSparseMatrix(x, y) {
      return algorithm08(x, y, rightLogShift, false);
    },
    'SparseMatrix, DenseMatrix': function SparseMatrixDenseMatrix(x, y) {
      return algorithm02(y, x, rightLogShift, true);
    },
    'DenseMatrix, SparseMatrix': function DenseMatrixSparseMatrix(x, y) {
      return algorithm01(x, y, rightLogShift, false);
    },
    'DenseMatrix, DenseMatrix': function DenseMatrixDenseMatrix(x, y) {
      return algorithm13(x, y, rightLogShift);
    },
    'Array, Array': function ArrayArray(x, y) {
      // use matrix implementation
      return rightLogShift(matrix(x), matrix(y)).valueOf();
    },
    'Array, Matrix': function ArrayMatrix(x, y) {
      // use matrix implementation
      return rightLogShift(matrix(x), y);
    },
    'Matrix, Array': function MatrixArray(x, y) {
      // use matrix implementation
      return rightLogShift(x, matrix(y));
    },
    'SparseMatrix, number | BigNumber': function SparseMatrixNumberBigNumber(x, y) {
      // check scalar
      if (equalScalar(y, 0)) {
        return x.clone();
      }

      return algorithm11(x, y, rightLogShift, false);
    },
    'DenseMatrix, number | BigNumber': function DenseMatrixNumberBigNumber(x, y) {
      // check scalar
      if (equalScalar(y, 0)) {
        return x.clone();
      }

      return algorithm14(x, y, rightLogShift, false);
    },
    'number | BigNumber, SparseMatrix': function numberBigNumberSparseMatrix(x, y) {
      // check scalar
      if (equalScalar(x, 0)) {
        return zeros(y.size(), y.storage());
      }

      return algorithm10(y, x, rightLogShift, true);
    },
    'number | BigNumber, DenseMatrix': function numberBigNumberDenseMatrix(x, y) {
      // check scalar
      if (equalScalar(x, 0)) {
        return zeros(y.size(), y.storage());
      }

      return algorithm14(y, x, rightLogShift, true);
    },
    'Array, number | BigNumber': function ArrayNumberBigNumber(x, y) {
      // use matrix implementation
      return rightLogShift(matrix(x), y).valueOf();
    },
    'number | BigNumber, Array': function numberBigNumberArray(x, y) {
      // use matrix implementation
      return rightLogShift(x, matrix(y)).valueOf();
    }
  });
  return rightLogShift;
});
exports.createRightLogShift = createRightLogShift;